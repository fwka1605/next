import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1101SectionMasterComponent } from './pb1101-section-master.component';

describe('Pb1101SectionMasterComponent', () => {
  let component: Pb1101SectionMasterComponent;
  let fixture: ComponentFixture<Pb1101SectionMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1101SectionMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1101SectionMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
