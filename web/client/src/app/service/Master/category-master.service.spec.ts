import { TestBed } from '@angular/core/testing';

import { CategoryMasterService } from './category-master.service';

describe('CategoryMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CategoryMasterService = TestBed.get(CategoryMasterService);
    expect(service).toBeTruthy();
  });
});
