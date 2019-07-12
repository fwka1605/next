import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc0601BillingJournalizingComponent } from './pc0601-billing-journalizing.component';

describe('Pc0601BillingJournalizingComponent', () => {
  let component: Pc0601BillingJournalizingComponent;
  let fixture: ComponentFixture<Pc0601BillingJournalizingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc0601BillingJournalizingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0601BillingJournalizingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
