import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pf0501CustomerLedgerComponent } from './pf0501-customer-ledger.component';

describe('Pf0501CustomerLedgerComponent', () => {
  let component: Pf0501CustomerLedgerComponent;
  let fixture: ComponentFixture<Pf0501CustomerLedgerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pf0501CustomerLedgerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pf0501CustomerLedgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
