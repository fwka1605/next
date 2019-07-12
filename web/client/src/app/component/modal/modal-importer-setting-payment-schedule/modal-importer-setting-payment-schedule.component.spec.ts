import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalImporterSettingPaymentScheduleComponent } from './modal-importer-setting-payment-schedule.component';

describe('ModalImporterSettingPaymentScheduleComponent', () => {
  let component: ModalImporterSettingPaymentScheduleComponent;
  let fixture: ComponentFixture<ModalImporterSettingPaymentScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalImporterSettingPaymentScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalImporterSettingPaymentScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
