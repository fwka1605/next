namespace Rac.VOne.Common.DataHandling
{
    /// <summary>端数処理種別</summary>
    public enum RoundingType
    {
        /// <summary>0:切り捨て</summary>
        Floor = 0,
        /// <summary>1:切り上げ</summary>
        Ceil,
        /// <summary>2:四捨五入</summary>
        Round,
        /// <summary>3:取込不可</summary>
        Error,
    }

}