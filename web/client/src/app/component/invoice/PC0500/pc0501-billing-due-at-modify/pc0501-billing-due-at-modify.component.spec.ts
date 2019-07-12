import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc0501BillingDueAtModifyComponent } from './pc0501-billing-due-at-modify.component';

describe('Pc0501BillingDueAtModifyComponent', () => {
  let component: Pc0501BillingDueAtModifyComponent;
  let fixture: ComponentFixture<Pc0501BillingDueAtModifyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc0501BillingDueAtModifyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0501BillingDueAtModifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
