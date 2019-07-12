import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe0201MatchingJournalizingComponent } from './pe0201-matching-journalizing.component';

describe('Pe0201MatchingJournalizingComponent', () => {
  let component: Pe0201MatchingJournalizingComponent;
  let fixture: ComponentFixture<Pe0201MatchingJournalizingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pe0201MatchingJournalizingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe0201MatchingJournalizingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
