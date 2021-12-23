classdef SparseMatrixBuilder
    
    properties (Access=private)
        Base
    end
    
    methods (Access=public)
        
        % class constructor
        function this = SparseMatrixBuilder(rows, cols)
            this.Base = SparseLibrary.SparseMatrixBuilder(rows, cols);
        end
        
        % get an element from the matrix by indexing (i, j)
        function val = Get(this, row, col)
            val = double( this.Base.Get(row - 1, col - 1) )'; % 1 to 0 based indexing
        end
        
        % set an element from the matrix by indexing (i, j)
        function Set(this, row, col, val)
            this.Base.Set(row - 1, col - 1, val); % 1 to 0 based indexing
        end
        
        % add a value to a matrix element by indexing (i, j)
        function Add(this, row, col, val)
            this.Base.Add(row - 1, col - 1, val); % 1 to 0 based indexing
        end
        
        % set all elements in the diagonal
        function SetDiagonal(this, val)
            this.Base.SetDiagonal(val);
        end
        
        % get the diagonal
        function output = GetDiagonal(this)
            diagonal = this.Base.GetDiagonal();
            output = double(diagonal)';
        end
        
        % get a row
        function output = GetRow(this, row)
            output = double( this.Base.GetRow(row - 1) ); % 1 to 0 based indexing
        end
        
        % set a row
        function SetRow(this, row, val)
            this.Base.SetRow(row - 1, val); % 1 to 0 based indexing
        end
        
        % get a column
        function output = GetColumn(this, col)
            output = double( this.Base.GetColumn(col - 1) )'; % 1 to 0 based indexing
        end
        
        % set a column
        function SetColumn(this, col, val)
            this.Base.SetColumn(col - 1, val); % 1 to 0 based indexing
        end
        
        % to dense
        function output = ToDense(this)
            dense = this.Base.ToDense();
            output = double(dense);
        end
        
        % to COO sparse matrix
        function output = ToCOO(this)
            coo = this.Base.ToCOO(1);
            output = [
                double(coo.Item1)
                double(coo.Item2)
                double(coo.Item3)
            ]';
        end
        
        % to matlab sparse
        function output = ToMatlabSparse(this)
            coo = this.ToCOO();
            m = double(this.Base.NumberOfRows);
            n = double(this.Base.NumberOfColumns);
            output = sparse(coo(:, 1), coo(:, 2), coo(:, 3), m, n);
        end
        
    end
    
end
