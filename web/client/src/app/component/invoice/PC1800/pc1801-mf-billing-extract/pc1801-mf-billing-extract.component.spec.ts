import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc1801MfBillingExtractComponent } from './pc1801-mf-billing-extract.component';

describe('Pc1801MfBillingExtractComponent', () => {
  let component: Pc1801MfBillingExtractComponent;
  let fixture: ComponentFixture<Pc1801MfBillingExtractComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc1801MfBillingExtractComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc1801MfBillingExtractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
