import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalIndividualMemoComponent } from './modal-individual-memo.component';

describe('ModalIndividualMemoComponent', () => {
  let component: ModalIndividualMemoComponent;
  let fixture: ComponentFixture<ModalIndividualMemoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalIndividualMemoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalIndividualMemoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
