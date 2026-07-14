export interface Task {
  id: number;
  title: string;
  description: string | null;
  isCompleted: boolean;
  dueDate: string | null;
  createdAt: string;
  categoryId: number | null;
  categoryName: string | null;
}

export interface CreateTaskRequest {
  title: string;
  description?: string | null;
  dueDate?: string | null;
  categoryId?: number | null;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string | null;
  isCompleted: boolean;
  dueDate?: string | null;
  categoryId?: number | null;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface TaskQueryParams {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  categoryId?: number;
}
