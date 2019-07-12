'use strict';

/**
 * accordion control
 */
const accordionBody = document.querySelectorAll('[data-accordion-body]')
const accordionTrigger = document.querySelectorAll('[data-accordion-trigger]')

if (accordionTrigger.length != 0) {
  // heigth get height Zero
  let targetElmHeight;
  Array.from(accordionBody, targetElm => {
    targetElmHeight = targetElm.offsetHeight === 0 ? '250' : targetElm.offsetHeight;
    targetElm.dataset.originalHeight = targetElmHeight;
    targetElm.style.height = '0px'
  })
  const heightControl = (targetElm, controlClass) => {
    if (targetElm.classList.contains(controlClass)) {
      targetElm.style.height = '0px';
    } else {
      targetElm.style.height = targetElm.dataset.originalHeight;
    }
  }

  const accordionControl = (triggerElm) => {
    Array.from(accordionBody, targetElm => {
      if (targetElm.dataset.accordionBody !== triggerElm.dataset.accordionTrigger) {
        targetElm.classList.remove('is-accordion-open');
        targetElm.style.height = '0px';
      } else if (targetElm.dataset.accordionBody === triggerElm.dataset.accordionTrigger) {
        heightControl(targetElm, 'is-accordion-open')
        targetElm.classList.toggle('is-accordion-open')
      }
    });
    triggerElm.parentNode.classList.toggle('is-accordion-open')
  }

  Array.from(accordionTrigger, triggerElm => {
    triggerElm.addEventListener('click', () => {
      accordionControl(triggerElm)
    })
  }, false)
}
