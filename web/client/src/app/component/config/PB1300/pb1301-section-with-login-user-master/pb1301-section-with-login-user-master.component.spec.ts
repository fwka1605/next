import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1301SectionWithLoginUserMasterComponent } from './pb1301-section-with-login-user-master.component';

describe('Pb1301SectionWithLoginUserMasterComponent', () => {
  let component: Pb1301SectionWithLoginUserMasterComponent;
  let fixture: ComponentFixture<Pb1301SectionWithLoginUserMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1301SectionWithLoginUserMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1301SectionWithLoginUserMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
