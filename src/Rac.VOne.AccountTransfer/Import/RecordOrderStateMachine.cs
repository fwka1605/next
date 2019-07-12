using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import
{
    /// <summary>
    /// レコード順序管理／チェック用のステートマシン
    /// </summary>
    public class RecordOrderStateMachine
    {
        public RecordOrderState Status { get; private set; }

        public RecordSetType RecordSetType { get; private set; }
        public bool AllowMultiHeader { get; private set; }

        public RecordOrderStateMachine(RecordSetType recordSetType, bool allowMultiHeader)
        {
            RecordSetType = recordSetType;
            AllowMultiHeader = allowMultiHeader;

            Status = RecordOrderState.Start;
        }

        /// <summary>
        /// 自動単体テスト用のコンストラクタ
        /// </summary>
        public RecordOrderStateMachine(RecordSetType recordSetType, bool allowMultiHeader, RecordOrderState state)
        {
            RecordSetType = recordSetType;
            AllowMultiHeader = allowMultiHeader;
            Status = state;
        }

        /// <param name="next">新たに読み込んだレコードの種類</param>
        public RecordOrderStateTransitionResult ChangeState(RecordType next)
        {
            switch (Status)
            {
                case RecordOrderState.Start:
                    return ChangeStateFromStart(next);
                case RecordOrderState.Header:
                    return ChangeStateFromHeader(next);
                case RecordOrderState.Data:
                    return ChangeStateFromData(next);
                case RecordOrderState.Trailer:
                    return ChangeStateFromTrailer(next);
                case RecordOrderState.End:
                    return ChangeStateFromEnd(next);
                case RecordOrderState.Completed:
                    Status = RecordOrderState.Error;
                    throw new InvalidOperationException("AlreadyCompleted"); // ここに来る場合は入力ファイルの問題ではなくバグ
                case RecordOrderState.Error:
                    Status = RecordOrderState.Error;
                    throw new InvalidOperationException("AlreadyError");     // 同上
                default:
                    Status = RecordOrderState.Error;
                    throw new NotImplementedException($"next = {next}");
            }
        }

        private RecordOrderStateTransitionResult ChangeStateFromStart(RecordType next)
        {
            switch (next)
            {
                case RecordType.Header:
                    Status = RecordOrderState.Header;
                    return RecordOrderStateTransitionResult.Success;

                case RecordType.Data:
                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoHeader;

                case RecordType.Trailer:
                    if (RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.TrailerNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoHeader;

                case RecordType.End:
                    if (RecordSetType == RecordSetType.HDT || RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.EndNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoHeader;

                case RecordType.EOF:
                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoHeader;

                default:
                    Status = RecordOrderState.Error;
                    throw new NotImplementedException($"next = {next}");
            }
        }

        private RecordOrderStateTransitionResult ChangeStateFromHeader(RecordType next)
        {
            switch (next)
            {
                case RecordType.Header:
                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.DuplicateHeader;

                case RecordType.Data:
                    Status = RecordOrderState.Data;
                    return RecordOrderStateTransitionResult.Success;

                case RecordType.Trailer:
                    if (RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.TrailerNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoData;

                case RecordType.End:
                    if (RecordSetType == RecordSetType.HDT || RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.EndNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoData;

                case RecordType.EOF:
                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoData;

                default:
                    Status = RecordOrderState.Error;
                    throw new NotImplementedException($"next = {next}");
            }
        }

        private RecordOrderStateTransitionResult ChangeStateFromData(RecordType next)
        {
            switch (next)
            {
                case RecordType.Header:
                    if (!AllowMultiHeader)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.MultiHeaderNotSupported;
                    }

                    if (RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Header;
                        return RecordOrderStateTransitionResult.Success;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoTrailer;

                case RecordType.Data:
                    Status = RecordOrderState.Data;
                    return RecordOrderStateTransitionResult.Success;

                case RecordType.Trailer:
                    if (RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.TrailerNotSupported;
                    }

                    Status = RecordOrderState.Trailer;
                    return RecordOrderStateTransitionResult.Success;

                case RecordType.End:
                    if (RecordSetType == RecordSetType.HDT || RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.EndNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoTrailer;

                case RecordType.EOF:
                    if (RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Completed;
                        return RecordOrderStateTransitionResult.Success;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoTrailer;

                default:
                    Status = RecordOrderState.Error;
                    throw new NotImplementedException($"next = {next}");
            }
        }

        private RecordOrderStateTransitionResult ChangeStateFromTrailer(RecordType next)
        {
            switch (next)
            {
                case RecordType.Header:
                    if (!AllowMultiHeader)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.MultiHeaderNotSupported;
                    }

                    // HDT/HDTE
                    Status = RecordOrderState.Header;
                    return RecordOrderStateTransitionResult.Success;

                case RecordType.Data:
                    if (!AllowMultiHeader)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.MultiHeaderNotSupported;
                    }

                    if (RecordSetType == RecordSetType.HDT)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.NoHeader;
                    }

                    // HDTE
                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoEnd;

                case RecordType.Trailer:
                    if (RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.TrailerNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.DuplicateTrailer;

                case RecordType.End:
                    if (RecordSetType == RecordSetType.HDT || RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.EndNotSupported;
                    }

                    Status = RecordOrderState.End;
                    return RecordOrderStateTransitionResult.Success;

                case RecordType.EOF:
                    if (RecordSetType == RecordSetType.HDT)
                    {
                        Status = RecordOrderState.Completed;
                        return RecordOrderStateTransitionResult.Success;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoEnd;

                default:
                    Status = RecordOrderState.Error;
                    throw new NotImplementedException($"next = {next}");
            }
        }

        private RecordOrderStateTransitionResult ChangeStateFromEnd(RecordType next)
        {
            switch (next)
            {
                case RecordType.Header:
                    if (!AllowMultiHeader)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.MultiHeaderNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.AlreadyEnded;

                case RecordType.Data:
                    if (!AllowMultiHeader)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.MultiHeaderNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoHeader;

                case RecordType.Trailer:
                    if (RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.TrailerNotSupported;
                    }
                    if (!AllowMultiHeader)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.MultiHeaderNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.NoHeader;


                case RecordType.End:
                    if (RecordSetType == RecordSetType.HDT || RecordSetType == RecordSetType.HD)
                    {
                        Status = RecordOrderState.Error;
                        return RecordOrderStateTransitionResult.EndNotSupported;
                    }

                    Status = RecordOrderState.Error;
                    return RecordOrderStateTransitionResult.DuplicateEnd;

                case RecordType.EOF:
                    Status = RecordOrderState.Completed;
                    return RecordOrderStateTransitionResult.Success;

                default:
                    Status = RecordOrderState.Error;
                    throw new NotImplementedException($"next = {next}");
            }
        }
    }
}

/* 別プロジェクトに作成した単体テストコード。1+6クラス。
 * FluentAssertionsを使用。
 * MH: MultiHeader
 * SH: SingleHeader
 * HDTE: Header, Data, Trailer, End

using FluentAssertions;
using RecordStateMachineProto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStateMachineProtoTest.Tests
{
    public abstract class TRecordOrderStateMachineBase
    {
        protected void Test(RecordState currentState, RecordType nextRecord, RecordState expectedState, RecordOrderStateTransitionResult expectedResult)
        {
            var machine = GetRecordOrderStateMachine(currentState);
            machine.ChangeState(nextRecord).Should().Be(expectedResult);
            machine.Status.Should().Be(expectedState);
        }

        protected void TestException(RecordState currentState, RecordType nextRecord, string expectedExceptionMessage)
        {
            var machine = GetRecordOrderStateMachine(currentState);
            var test = new Action(() => machine.ChangeState(nextRecord));
            test.ShouldThrow<InvalidOperationException>().WithMessage(expectedExceptionMessage);
            machine.Status.Should().Be(RecordOrderState.Error);
        }

        protected abstract RecordOrderStateMachine GetRecordOrderStateMachine(RecordState state);
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordStateMachineProto.Models;
using System;

namespace RecordStateMachineProtoTest.Tests
{
    [TestClass]
    public class TRecordOrderStateMachine_MH_HD : TRecordOrderStateMachineBase
    {
        protected override RecordOrderStateMachine GetRecordOrderStateMachine(RecordState state)
        {
            return new RecordOrderStateMachine(RecordSetType.HD, true, state);
        }

        [TestMethod]
        public void ChangeStateFromStart()
        {
            var state = RecordOrderState.Start;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.TrailerNotSupported);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
        }

        [TestMethod]
        public void ChangeStateFromHeader()
        {
            var state = RecordOrderState.Header;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateHeader);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.TrailerNotSupported);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
        }

        [TestMethod]
        public void ChangeStateFromData()
        {
            var state = RecordOrderState.Data;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.TrailerNotSupported);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Completed, RecordOrderStateTransitionResult.Success);
        }

        [TestMethod]
        public void ChangeStateFromCompleted()
        {
            var msg = "AlreadyCompleted";
            var state = RecordOrderState.Completed;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }

        [TestMethod]
        public void ChangeStateFromError()
        {
            var msg = "AlreadyError";
            var state = RecordOrderState.Error;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordStateMachineProto.Models;
using System;

namespace RecordStateMachineProtoTest.Tests
{
    [TestClass]
    public class TRecordOrderStateMachine_MH_HDT : TRecordOrderStateMachineBase
    {
        protected override RecordOrderStateMachine GetRecordOrderStateMachine(RecordState state)
        {
            return new RecordOrderStateMachine(RecordSetType.HDT, true, state);
        }

        [TestMethod]
        public void ChangeStateFromStart()
        {
            var state = RecordOrderState.Start;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
        }

        [TestMethod]
        public void ChangeStateFromHeader()
        {
            var state = RecordOrderState.Header;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateHeader);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
        }

        [TestMethod]
        public void ChangeStateFromData()
        {
            var state = RecordOrderState.Data;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Trailer, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
        }

        [TestMethod]
        public void ChangeStateFromTrailer()
        {
            var state = RecordOrderState.Trailer;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateTrailer);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Completed, RecordOrderStateTransitionResult.Success);
        }

        [TestMethod]
        public void ChangeStateFromCompleted()
        {
            var msg = "AlreadyCompleted";
            var state = RecordOrderState.Completed;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }

        [TestMethod]
        public void ChangeStateFromError()
        {
            var msg = "AlreadyError";
            var state = RecordOrderState.Error;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordStateMachineProto.Models;
using System;

namespace RecordStateMachineProtoTest.Tests
{
    [TestClass]
    public class TRecordOrderStateMachine_MH_HDTE : TRecordOrderStateMachineBase
    {
        protected override RecordOrderStateMachine GetRecordOrderStateMachine(RecordState state)
        {
            return new RecordOrderStateMachine(RecordSetType.HDTE, true, state);
        }

        [TestMethod]
        public void ChangeStateFromStart()
        {
            var state = RecordOrderState.Start;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
        }

        [TestMethod]
        public void ChangeStateFromHeader()
        {
            var state = RecordOrderState.Header;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateHeader);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
        }

        [TestMethod]
        public void ChangeStateFromData()
        {
            var state = RecordOrderState.Data;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Trailer, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
        }

        [TestMethod]
        public void ChangeStateFromTrailer()
        {
            var state = RecordOrderState.Trailer;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoEnd);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateTrailer);
            Test(state, RecordType.End, RecordOrderState.End, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoEnd);
        }

        [TestMethod]
        public void ChangeStateFromEnd()
        {
            var state = RecordOrderState.End;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.AlreadyEnded);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateEnd);
            Test(state, RecordType.EOF, RecordOrderState.Completed, RecordOrderStateTransitionResult.Success);
        }

        [TestMethod]
        public void ChangeStateFromCompleted()
        {
            var msg = "AlreadyCompleted";
            var state = RecordOrderState.Completed;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }

        [TestMethod]
        public void ChangeStateFromError()
        {
            var msg = "AlreadyError";
            var state = RecordOrderState.Error;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordStateMachineProto.Models;
using System;

namespace RecordStateMachineProtoTest.Tests
{
    [TestClass]
    public class TRecordOrderStateMachine_SH_HD : TRecordOrderStateMachineBase
    {
        protected override RecordOrderStateMachine GetRecordOrderStateMachine(RecordState state)
        {
            return new RecordOrderStateMachine(RecordSetType.HD, false, state);
        }

        [TestMethod]
        public void ChangeStateFromStart()
        {
            var state = RecordOrderState.Start;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.TrailerNotSupported);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
        }

        [TestMethod]
        public void ChangeStateFromHeader()
        {
            var state = RecordOrderState.Header;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateHeader);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.TrailerNotSupported);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
        }

        [TestMethod]
        public void ChangeStateFromData()
        {
            var state = RecordOrderState.Data;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.TrailerNotSupported);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Completed, RecordOrderStateTransitionResult.Success);
        }

        [TestMethod]
        public void ChangeStateFromCompleted()
        {
            var msg = "AlreadyCompleted";
            var state = RecordOrderState.Completed;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }

        [TestMethod]
        public void ChangeStateFromError()
        {
            var msg = "AlreadyError";
            var state = RecordOrderState.Error;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordStateMachineProto.Models;
using System;

namespace RecordStateMachineProtoTest.Tests
{
    [TestClass]
    public class TRecordOrderStateMachine_SH_HDT : TRecordOrderStateMachineBase
    {
        protected override RecordOrderStateMachine GetRecordOrderStateMachine(RecordState state)
        {
            return new RecordOrderStateMachine(RecordSetType.HDT, false, state);
        }

        [TestMethod]
        public void ChangeStateFromStart()
        {
            var state = RecordOrderState.Start;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
        }

        [TestMethod]
        public void ChangeStateFromHeader()
        {
            var state = RecordOrderState.Header;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateHeader);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
        }

        [TestMethod]
        public void ChangeStateFromData()
        {
            var state = RecordOrderState.Data;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Trailer, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
        }

        [TestMethod]
        public void ChangeStateFromTrailer()
        {
            var state = RecordOrderState.Trailer;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateTrailer);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.EndNotSupported);
            Test(state, RecordType.EOF, RecordOrderState.Completed, RecordOrderStateTransitionResult.Success);
        }

        [TestMethod]
        public void ChangeStateFromCompleted()
        {
            var msg = "AlreadyCompleted";
            var state = RecordOrderState.Completed;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }

        [TestMethod]
        public void ChangeStateFromError()
        {
            var msg = "AlreadyError";
            var state = RecordOrderState.Error;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordStateMachineProto.Models;
using System;

namespace RecordStateMachineProtoTest.Tests
{
    [TestClass]
    public class TRecordOrderStateMachine_SH_HDTE : TRecordOrderStateMachineBase
    {
        protected override RecordOrderStateMachine GetRecordOrderStateMachine(RecordState state)
        {
            return new RecordOrderStateMachine(RecordSetType.HDTE, false, state);
        }

        [TestMethod]
        public void ChangeStateFromStart()
        {
            var state = RecordOrderState.Start;

            Test(state, RecordType.Header, RecordOrderState.Header, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoHeader);
        }

        [TestMethod]
        public void ChangeStateFromHeader()
        {
            var state = RecordOrderState.Header;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateHeader);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoData);
        }

        [TestMethod]
        public void ChangeStateFromData()
        {
            var state = RecordOrderState.Data;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Data, RecordOrderState.Data, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.Trailer, RecordOrderState.Trailer, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoTrailer);
        }

        [TestMethod]
        public void ChangeStateFromTrailer()
        {
            var state = RecordOrderState.Trailer;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateTrailer);
            Test(state, RecordType.End, RecordOrderState.End, RecordOrderStateTransitionResult.Success);
            Test(state, RecordType.EOF, RecordOrderState.Error, RecordOrderStateTransitionResult.NoEnd);
        }

        [TestMethod]
        public void ChangeStateFromEnd()
        {
            var state = RecordOrderState.End;

            Test(state, RecordType.Header, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Data, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.Trailer, RecordOrderState.Error, RecordOrderStateTransitionResult.MultiHeaderNotSupported);
            Test(state, RecordType.End, RecordOrderState.Error, RecordOrderStateTransitionResult.DuplicateEnd);
            Test(state, RecordType.EOF, RecordOrderState.Completed, RecordOrderStateTransitionResult.Success);
        }

        [TestMethod]
        public void ChangeStateFromCompleted()
        {
            var msg = "AlreadyCompleted";
            var state = RecordOrderState.Completed;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }

        [TestMethod]
        public void ChangeStateFromError()
        {
            var msg = "AlreadyError";
            var state = RecordOrderState.Error;

            TestException(state, RecordType.Header, msg);
            TestException(state, RecordType.Data, msg);
            TestException(state, RecordType.Trailer, msg);
            TestException(state, RecordType.End, msg);
            TestException(state, RecordType.EOF, msg);
        }
    }
}

*/
