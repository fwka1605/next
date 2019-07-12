import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalSelectParentCustomerComponent } from './modal-select-parent-customer.component';

describe('ModalSelectParentCustomerComponent', () => {
  let component: ModalSelectParentCustomerComponent;
  let fixture: ComponentFixture<ModalSelectParentCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalSelectParentCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalSelectParentCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
