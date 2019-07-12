import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0701AccountTitleMasterComponent } from './pb0701-account-title-master.component';

describe('Pb0701AccountTitleMasterComponent', () => {
  let component: Pb0701AccountTitleMasterComponent;
  let fixture: ComponentFixture<Pb0701AccountTitleMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0701AccountTitleMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0701AccountTitleMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
