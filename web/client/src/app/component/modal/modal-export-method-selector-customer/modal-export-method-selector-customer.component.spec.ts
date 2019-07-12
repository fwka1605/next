import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalExportMethodSelectorCustomerComponent } from './modal-export-method-selector-customer.component';

describe('ModalExportMethodSelectorCustomerComponent', () => {
  let component: ModalExportMethodSelectorCustomerComponent;
  let fixture: ComponentFixture<ModalExportMethodSelectorCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalExportMethodSelectorCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalExportMethodSelectorCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
