import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc0901PaymentScheduleInputComponent } from './pc0901-payment-schedule-input.component';

describe('Pc0901PaymentScheduleInputComponent', () => {
  let component: Pc0901PaymentScheduleInputComponent;
  let fixture: ComponentFixture<Pc0901PaymentScheduleInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc0901PaymentScheduleInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0901PaymentScheduleInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
