import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph2001MfAggrHistorySearchComponent } from './ph2001-mf-aggr-history-search.component';

describe('Ph2001MfAggrHistorySearchComponent', () => {
  let component: Ph2001MfAggrHistorySearchComponent;
  let fixture: ComponentFixture<Ph2001MfAggrHistorySearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph2001MfAggrHistorySearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph2001MfAggrHistorySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
