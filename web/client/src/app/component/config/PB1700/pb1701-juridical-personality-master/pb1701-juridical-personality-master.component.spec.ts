import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1701JuridicalPersonalityMasterComponent } from './pb1701-juridical-personality-master.component';

describe('Pb1701JuridicalPersonalityMasterComponent', () => {
  let component: Pb1701JuridicalPersonalityMasterComponent;
  let fixture: ComponentFixture<Pb1701JuridicalPersonalityMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1701JuridicalPersonalityMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1701JuridicalPersonalityMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
