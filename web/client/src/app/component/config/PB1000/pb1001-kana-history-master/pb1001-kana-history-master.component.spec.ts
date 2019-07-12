import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1001KanaHistoryMasterComponent } from './pb1001-kana-history-master.component';

describe('Pb1001KanaHistoryMasterComponent', () => {
  let component: Pb1001KanaHistoryMasterComponent;
  let fixture: ComponentFixture<Pb1001KanaHistoryMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1001KanaHistoryMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1001KanaHistoryMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
