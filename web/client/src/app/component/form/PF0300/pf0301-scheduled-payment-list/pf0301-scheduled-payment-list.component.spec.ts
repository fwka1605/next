import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pf0301ScheduledPaymentListComponent } from './pf0301-scheduled-payment-list.component';

describe('Pf0301ScheduledPaymentListComponent', () => {
  let component: Pf0301ScheduledPaymentListComponent;
  let fixture: ComponentFixture<Pf0301ScheduledPaymentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pf0301ScheduledPaymentListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pf0301ScheduledPaymentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
