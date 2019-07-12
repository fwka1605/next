import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMatchingRecordedAtComponent } from './modal-matching-recorded-at.component';

describe('ModalMatchingRecordedAtComponent', () => {
  let component: ModalMatchingRecordedAtComponent;
  let fixture: ComponentFixture<ModalMatchingRecordedAtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalMatchingRecordedAtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMatchingRecordedAtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
