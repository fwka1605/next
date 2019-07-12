import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe1001MfMatchingOutputComponent } from './pe1001-mf-matching-output.component';

describe('Pe1001MfMatchingOutputComponent', () => {
  let component: Pe1001MfMatchingOutputComponent;
  let fixture: ComponentFixture<Pe1001MfMatchingOutputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pe1001MfMatchingOutputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe1001MfMatchingOutputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
