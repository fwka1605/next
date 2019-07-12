import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalImportMethodSelectorCustomerComponent } from './modal-import-method-selector-customer.component';

describe('ModalImportMethodSelectorCustomerComponent', () => {
  let component: ModalImportMethodSelectorCustomerComponent;
  let fixture: ComponentFixture<ModalImportMethodSelectorCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalImportMethodSelectorCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalImportMethodSelectorCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
