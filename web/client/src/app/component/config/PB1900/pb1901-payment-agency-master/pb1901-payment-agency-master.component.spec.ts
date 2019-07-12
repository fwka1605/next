import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1901PaymentAgencyMasterComponent } from './pb1901-payment-agency-master.component';

describe('Pb1901PaymentAgencyMasterComponent', () => {
  let component: Pb1901PaymentAgencyMasterComponent;
  let fixture: ComponentFixture<Pb1901PaymentAgencyMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1901PaymentAgencyMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1901PaymentAgencyMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
