import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe0101MatchingSequentialComponent } from './pe0101-matching-sequential.component';

describe('Pe0101MatchingSequentialComponent', () => {
  let component: Pe0101MatchingSequentialComponent;
  let fixture: ComponentFixture<Pe0101MatchingSequentialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [Pe0101MatchingSequentialComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe0101MatchingSequentialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
