import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe0301MatchingHistorySearchComponent } from './pe0301-matching-history-search.component';

describe('Pe0301MatchingHistorySearchComponent', () => {
  let component: Pe0301MatchingHistorySearchComponent;
  let fixture: ComponentFixture<Pe0301MatchingHistorySearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pe0301MatchingHistorySearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe0301MatchingHistorySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
