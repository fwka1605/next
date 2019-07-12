/**
 * テーブルヘッダーのソートボタンクリック時のソートリスト表示
 */
class sortListShow {
  constructor() {
    this.sortBtnBind()
  }
  sortBtnBind() {
    const sortBtns = document.querySelectorAll('[data-sort-btn]')

    if (sortBtns.length != 0) {
      Array.from(sortBtns, btnElm => {
        btnElm.addEventListener('click', () => {
          btnElm.nextElementSibling.classList.toggle('is-show')
        })
      })
    }
  }
}

const sortList = new sortListShow();


/**
 * table の一行選択のスタイル変更
 */

class tableLineSelect {
  constructor() {
    this.lineHeadBind()
  }

  lineHeadBind() {
    const lineHeadCells = document.querySelectorAll('.line-selector')
    Array.from(lineHeadCells, lineHeadCell => {
      lineHeadCell.addEventListener('click', () => {
        lineHeadCell.parentNode.classList.toggle('is-line-selected')
      })
    })
  }
}

const lineSelect = new tableLineSelect();
