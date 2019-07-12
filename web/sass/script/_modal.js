class ModalInit {
  constructor() {
    this.modalEventBind();
  }


  // background scroll stop
  bgScrollStop() {
    document.body.classList.toggle('no-scroll');
  }

  modalEventBind() {
    /** 
     * modal control
     */
    const modalBody = document.getElementById('modal-body');
    const modalContents = document.querySelectorAll('[data-modal-contents]');
    let modalBackground;
    const modalTrigger = document.querySelectorAll('[data-modal]');
    const modalCloseBtn = document.querySelectorAll('[data-modal-close]');
    const modalShowClass = 'is-show';

    if (modalTrigger.length != 0) {
      //make modal background cover
      const makeModalBg = () => {
        const modalBg = document.createElement('div');
        document.body.appendChild(modalBg);
        modalBg.setAttribute('id', 'modal-cover')
        modalBackground = document.getElementById('modal-cover');
      }
      makeModalBg();

      //multi modal content
      const modalContentsOpen = (triggerDate) => {
        Array.from(modalContents, contentsElm => {
          if (contentsElm.dataset.modalContents === triggerDate) {
            contentsElm.classList.add(modalShowClass)
          } else {
            contentsElm.classList.remove(modalShowClass)
          }
        })
      }

      // modal open
      Array.from(modalTrigger, triggerElm => {
        triggerElm.addEventListener('click', () => {
          modalBody.classList.add(modalShowClass);
          if (triggerElm.dataset.modal) {
            modalContentsOpen(triggerElm.dataset.modal);
          }
          modalBackground.classList.add(modalShowClass);
          this.bgScrollStop()
        })
      }, false)

      // background clickable modal close 
      modalBackground.addEventListener('click', () => {
        modalBackground.classList.remove(modalShowClass)
        modalBody.classList.remove(modalShowClass)
        this.bgScrollStop()
      })

      // modal close btn
      Array.from(modalCloseBtn, btnElm => {
        btnElm.addEventListener('click', () => {
          modalBody.classList.remove(modalShowClass);
          const currentShowContetnt = document.querySelector('.is-show[data-modal-contents]');
          currentShowContetnt.classList.remove(modalShowClass);
          modalBackground.classList.remove(modalShowClass);
          this.bgScrollStop()
        })
      }, false)
    }
  }
}

const modal = new ModalInit();
