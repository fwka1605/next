import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe0102MatchingIndividualComponent } from './pe0102-matching-individual.component';

describe('Pe0102MatchingIndividualComponent', () => {
  let component: Pe0102MatchingIndividualComponent;
  let fixture: ComponentFixture<Pe0102MatchingIndividualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pe0102MatchingIndividualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe0102MatchingIndividualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
