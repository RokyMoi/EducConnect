export interface Pagination{
    CurrentPage: number;
    ItemsPerPage: number;
    TotalItems:number;
    TotalPages: number;
}

export class PaginationResult<T>{
    items?: T;
    pagination?: Pagination;

}