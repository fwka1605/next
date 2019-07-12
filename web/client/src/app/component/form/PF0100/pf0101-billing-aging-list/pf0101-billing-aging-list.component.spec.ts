import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pf0101BillingAgingListComponent } from './pf0101-billing-aging-list.component';

describe('Pf0101BillingAgingListComponent', () => {
  let component: Pf0101BillingAgingListComponent;
  let fixture: ComponentFixture<Pf0101BillingAgingListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pf0101BillingAgingListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pf0101BillingAgingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
