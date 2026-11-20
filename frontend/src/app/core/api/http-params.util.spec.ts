import { toFormData, toHttpParams } from './http-params.util';

describe('toHttpParams', () => {
  it('skips empty values and repeats array keys', () => {
    const params = toHttpParams({ search: 'c#', page: 2, empty: '', none: null, ids: [1, 2] });
    expect(params.toString()).toBe('search=c%23&page=2&ids=1&ids=2');
  });
});

describe('toFormData', () => {
  it('builds ASP.NET-compatible multipart keys', () => {
    const file = new File(['x'], 'cover.png');
    const form = toFormData({ name: 'Web', keywords: ['a', 'b'], topics: [{ topic: 'HTML' }], cover: file });

    expect(form.get('name')).toBe('Web');
    expect(form.getAll('keywords')).toEqual(['a', 'b']);
    expect(form.get('topics[0].topic')).toBe('HTML');
    expect((form.get('cover') as File).name).toBe('cover.png');
  });
});
