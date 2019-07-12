class DragDropImporter {
  constructor() {
    this.dragDropDecide();
    this.standardUpload();
  }
  /**
   * input file onchange  
   */
  standardUpload() {
    const targetInput = document.querySelector('[data-dragable]');
    const coverElm = document.querySelector('[ data-dragable-result]');
    if (targetInput) {
      targetInput.addEventListener('change', () => {
        coverElm.style.backgoundColor = 'transparent';
        coverElm.innerHTML = '';
      });
    }
  }

  /** 
   * dragDrop file decide
   */
  dragDropDecide() {
    const ddElments = document.querySelectorAll('[data-dragable]');
    if (ddElments.length != 0) {
      this.ddAeraCreate()
    } else {
      return false
    }
  }

  /** 
   * dragDrop area assign
   */
  ddAeraCreate() {
    const ddArea = document.getElementById('main');
    const ddCover = document.createElement('input');
    ddCover.setAttribute('type', 'file');
    ddCover.setAttribute('value', '');
    ddCover.classList.add('dd-cover');
    ddArea.appendChild(ddCover);
    this.ddAeraBind(ddArea, ddCover)
  }

  /** 
 * dragDrop event bind
 */
  ddAeraBind(targetElm, controlElm) {
    targetElm.addEventListener('dragover', (event) => {
      event.preventDefault();
      // event.dataTransfer.dropEffect = 'copy';
      // console.log(event.dataTransfer.files)
      this.showDropCover(controlElm);
    }) //, { once: true }のoption入れるとファイル間違えたときに二度とできない
    controlElm.addEventListener('dragleave', (event) => {
      event.preventDefault();
      this.hideDropCover(controlElm);
    }) //, { once: true }のoption入れるとファイル間違えたときに二度とできない
    controlElm.addEventListener('drop', (event) => {
      event.preventDefault();
      this.hideDropCover(controlElm);
      const files = event.dataTransfer.files;
      this.fileInfoInsert(files)
    })
  }
  fileInfoInsert(filesData) {
    const ddElments = document.querySelector('[ data-dragable-result]');
    ddElments.innerHTML = filesData[0].name;
  }
  showDropCover(elm) {
    elm.classList.add('is-show')
  }
  hideDropCover(elm) {
    elm.classList.remove('is-show')
  }
}

const dragDropImpoterInit = new DragDropImporter();
