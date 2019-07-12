import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1501IgnoreKanaMasterComponent } from './pb1501-ignore-kana-master.component';

describe('Pb1501IgnoreKanaMasterComponent', () => {
  let component: Pb1501IgnoreKanaMasterComponent;
  let fixture: ComponentFixture<Pb1501IgnoreKanaMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1501IgnoreKanaMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1501IgnoreKanaMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
