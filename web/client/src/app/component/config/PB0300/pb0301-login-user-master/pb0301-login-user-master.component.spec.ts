import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0301LoginUserMasterComponent } from './pb0301-login-user-master.component';

describe('Pb0301LoginUserMasterComponent', () => {
  let component: Pb0301LoginUserMasterComponent;
  let fixture: ComponentFixture<Pb0301LoginUserMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0301LoginUserMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0301LoginUserMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
