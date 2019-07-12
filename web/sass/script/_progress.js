class progressPrototype {
  constructor() {
    this.progressEnd()
  }
  progressEnd() {
    const progressBar = document.querySelector('.progress-container__box__bottom__bar__inner');
    if (progressBar) {
      progressBar.addEventListener('animationend', () => {
        console.log(999);
        this.changeText();
      });
    }
  }
  changeText() {
    const progressIcon = document.querySelector('.progress-container__box__title__icon');
    const progressText = document.querySelector('.progress-container__box__title__text');
    progressIcon.innerHTML = '<img src="/assets/image/icon_completed.png" class="">';
    progressText.innerHTML = '全ての処理が完了しました。(ダミー)'
  }

}

const progressInit = new progressPrototype();
