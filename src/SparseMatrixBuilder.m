% Utility class for building large sparse matrices.
classdef SparseMatrixBuilder < handle

    % private properties
    properties (Access = private)
        rows % storage of the row indices of the non-zero elements
        cols % storage of the column indices of the non-zero elements
        vals % storage of the non-zero elements
        size % current storage size
    end

    % public properties
    properties (Access = public)
        n_rows % number of rows
        n_cols % number of columns
        n_nzrs % number of non-zero elements
    end

    % private methods
    methods (Access = private)

        % dynamically allocates more storage
        function self = resize(self, new_size)
            if new_size ~= self.size
                new_rows = zeros([new_size, 1], 'int32' );
                new_cols = zeros([new_size, 1], 'int32' );
                new_vals = zeros([new_size, 1], 'double');
                index = min(new_size, self.size);
                new_rows(1:index) = self.rows(1:index);
                new_cols(1:index) = self.cols(1:index);
                new_vals(1:index) = self.vals(1:index);
                self.rows = new_rows;
                self.cols = new_cols;
                self.vals = new_vals;
                self.size = new_size;
                self.n_nzrs = min(self.n_nzrs, self.size);
            end
        end

    end

    % public methods
    methods (Access = public)

        % constructor
        function self = SparseMatrixBuilder(n_rows, n_cols)
            self.n_rows = int32(n_rows);
            self.n_cols = int32(n_cols);
            self.rows = zeros([1, 1], 'int32' );
            self.cols = zeros([1, 1], 'int32' );
            self.vals = zeros([1, 1], 'double');
            self.n_nzrs = int32(0);
            self.size = int32(1);
        end

        % add a single value
        function self = add_val(self, i, j, v)
            if i < 1 || i > self.n_rows
                error('Row index out of range.');
            end
            if j < 1 || j > self.n_cols
                error('Column index out of range.');
            end
            if self.size == self.n_nzrs
                self.resize(self.size*2);
            end
            self.n_nzrs = self.n_nzrs + 1;
            self.rows(self.n_nzrs) =  int32(i);
            self.cols(self.n_nzrs) =  int32(j);
            self.vals(self.n_nzrs) = double(v);
        end

        % add multiple values
        function self = add_vals(self, i, j, v)
            if min(i) < 1 || max(i) > self.n_rows
                error('Row index out of range.');
            end
            if min(j) < 1 || max(j) > self.n_cols
                error('Column index out of range.');
            end
            count = length(v);
            if self.n_nzrs + count > self.size
                new_size = self.size;
                while self.n_nzrs + count > new_size
                    new_size = new_size*2;
                end
                self.resize(new_size);
            end
            self.rows(self.n_nzrs+1:self.n_nzrs+count) =  int32(i);
            self.cols(self.n_nzrs+1:self.n_nzrs+count) =  int32(j);
            self.vals(self.n_nzrs+1:self.n_nzrs+count) = double(v);
            self.n_nzrs = self.n_nzrs + count;
        end

        % removes excess storage (call after building)
        function self = squeeze(self)
            self.resize(self.n_nzrs);
        end

        % converts to matlab sparse matrix
        function out = to_matlab(self)
            out = sparse(self.rows,   ...
                         self.cols,   ...
                         self.vals,   ...
                         self.n_rows, ...
                         self.n_cols, ...
                         self.n_nzrs);
        end

    end

end
