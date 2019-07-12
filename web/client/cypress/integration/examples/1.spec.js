/// <reference types="Cypress" />

context('Actions', () => {

  beforeEach(() => {
    cy.visit('http://192.168.11.117:4200/')
  })


  it('.click() - click on a DOM element', () => {
    cy.get('.button--w100-login').click()
    cy.get(':nth-child(2) > .menu__first__item__icon > .menu__first__item__icon__img').click()
  })
 
  

})
