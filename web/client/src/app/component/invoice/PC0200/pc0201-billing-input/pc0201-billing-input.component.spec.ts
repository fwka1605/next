import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc0201BillingInputComponent } from './pc0201-billing-input.component';

describe('Pc0201BillingInputComponent', () => {
  let component: Pc0201BillingInputComponent;
  let fixture: ComponentFixture<Pc0201BillingInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc0201BillingInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0201BillingInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
