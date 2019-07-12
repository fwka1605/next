import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe0401MatchingJournalizingCancellationComponent } from './pe0401-matching-journalizing-cancellation.component';

describe('Pe0401MatchingJournalizingCancellationComponent', () => {
  let component: Pe0401MatchingJournalizingCancellationComponent;
  let fixture: ComponentFixture<Pe0401MatchingJournalizingCancellationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pe0401MatchingJournalizingCancellationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe0401MatchingJournalizingCancellationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
