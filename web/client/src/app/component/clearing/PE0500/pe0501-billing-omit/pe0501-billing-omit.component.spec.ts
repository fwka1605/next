import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe0501BillingOmitComponent } from './pe0501-billing-omit.component';

describe('Pe0501BillingOmitComponent', () => {
  let component: Pe0501BillingOmitComponent;
  let fixture: ComponentFixture<Pe0501BillingOmitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pe0501BillingOmitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe0501BillingOmitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
