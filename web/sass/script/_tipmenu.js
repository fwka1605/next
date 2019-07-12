class TipMenu {
  constructor() {
    this.tipControl()
  }
  /**
   * content header menu open
   */
  tipControl() {
    const otherBtn = document.querySelector('[data-tip-menu="btn"]');
    const otherMenu = document.querySelector('[data-tip-menu="body"]');
    if (otherBtn) {
      otherBtn.addEventListener('click', () => {
        otherMenu.classList.toggle('is-open');
      });
    }
  }

}

const tipMenu = new TipMenu();
