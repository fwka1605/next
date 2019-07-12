import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph0101SettingSsecurityComponent } from './ph0101-setting-ssecurity.component';

describe('Ph0101SettingSsecurityComponent', () => {
  let component: Ph0101SettingSsecurityComponent;
  let fixture: ComponentFixture<Ph0101SettingSsecurityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph0101SettingSsecurityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph0101SettingSsecurityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
