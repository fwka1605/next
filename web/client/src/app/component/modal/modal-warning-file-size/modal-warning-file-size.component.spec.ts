import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalWarningFileSizeComponent } from './modal-warning-file-size.component';

describe('ModalWarningFileSizeComponent', () => {
  let component: ModalWarningFileSizeComponent;
  let fixture: ComponentFixture<ModalWarningFileSizeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalWarningFileSizeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalWarningFileSizeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
