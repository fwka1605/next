import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalImporterSettingCustomerComponent } from './modal-importer-setting-customer.component';

describe('ModalImporterSettingCustomerComponent', () => {
  let component: ModalImporterSettingCustomerComponent;
  let fixture: ComponentFixture<ModalImporterSettingCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalImporterSettingCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalImporterSettingCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
