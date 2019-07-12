import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pj0201UserPasswordReissueComponent } from './pj0201-user-password-reissue.component';

describe('Pj0201UserPasswordReissueComponent', () => {
  let component: Pj0201UserPasswordReissueComponent;
  let fixture: ComponentFixture<Pj0201UserPasswordReissueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pj0201UserPasswordReissueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pj0201UserPasswordReissueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
