import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pf0102BillingAgingListDetailComponent } from './pf0102-billing-aging-list-detail.component';

describe('Pf0102BillingAgingListDetailComponent', () => {
  let component: Pf0102BillingAgingListDetailComponent;
  let fixture: ComponentFixture<Pf0102BillingAgingListDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pf0102BillingAgingListDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pf0102BillingAgingListDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
