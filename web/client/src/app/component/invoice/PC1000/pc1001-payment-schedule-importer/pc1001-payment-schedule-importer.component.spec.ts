import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc1001PaymentScheduleImporterComponent } from './pc1001-payment-schedule-importer.component';

describe('Pc1001PaymentScheduleImporterComponent', () => {
  let component: Pc1001PaymentScheduleImporterComponent;
  let fixture: ComponentFixture<Pc1001PaymentScheduleImporterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc1001PaymentScheduleImporterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc1001PaymentScheduleImporterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
