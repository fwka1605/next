import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc0301BillingSearchComponent } from './pc0301-billing-search.component';

describe('Pc0301BillingSearchComponent', () => {
  let component: Pc0301BillingSearchComponent;
  let fixture: ComponentFixture<Pc0301BillingSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc0301BillingSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0301BillingSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
