import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalConfirmMatchingAdvancedCustomerComponent } from './modal-confirm-matching-advanced-customer.component';

describe('ModalConfirmMatchingAdvancedCustomerComponent', () => {
  let component: ModalConfirmMatchingAdvancedCustomerComponent;
  let fixture: ComponentFixture<ModalConfirmMatchingAdvancedCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalConfirmMatchingAdvancedCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalConfirmMatchingAdvancedCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
