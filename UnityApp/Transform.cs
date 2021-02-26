using System.Collections.Generic;
using SoftRender.Engine;
using Mathlib;

namespace SoftRender.UnityApp
{
    public class Transform : Component
    {
        Transform   parent;
        Vector3     _localPosition = Vector3.zero;
        Quaternion  _localRotation = Quaternion.identity;
        Vector3     _localScale = Vector3.one;
        bool        recomputeMatrix = true;
        Matrix4x4   _localMatrix;
        Matrix4x4   _invLocalMatrix;

        List<Transform> _children = new List<Transform>();

        public List<Transform>  children
        {
            get => _children;
        }

        public Vector3 localPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                recomputeMatrix = true;
            }
        }
        public Vector3 position
        {
            get => (parent == null) ? (_localPosition) : (parent.localToWorldMatrix * _localPosition);
            set
            {
                if (parent == null)
                {
                    _localPosition = value;
                }
                else
                {
                    _localPosition = parent.worldToLocalMatrix * value;
                }
                recomputeMatrix = true;
            }
        }
        public Vector3 localScale
        {
            get => _localScale;
            set
            {
                _localScale = value;
                recomputeMatrix = true;
            }
        }
        public Vector3 scale
        {
            get => (parent == null) ? (_localPosition) : (parent.localToWorldMatrix * _localScale);
            set
            {
                if (parent == null)
                {
                    _localScale = value;
                }
                else
                {
                    _localScale = parent.worldToLocalMatrix * value;
                }
                recomputeMatrix = true;
            }
        }

        public Quaternion localRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;
                recomputeMatrix = true;
            }
        }
        public Quaternion rotation
        {
            get => (parent == null) ? (_localRotation) : parent.rotation * _localRotation;
            set
            {
                if (parent == null)
                {
                    _localRotation = value;
                }
                else
                {
                    _localRotation = -parent.rotation * value;
                }
                recomputeMatrix = true;
            }
        }

        public Matrix4x4 localToWorldMatrix
        {
            get
            {
                if (recomputeMatrix)
                {
                    _localMatrix = Matrix4x4.PRS(_localPosition, _localRotation, _localScale);
                    _invLocalMatrix = _localMatrix.inverse;
                    recomputeMatrix = false;
                }
                if (parent == null)
                {
                    return _localMatrix;
                }
                return parent.localToWorldMatrix * _localMatrix;
            }
        }

        public Matrix4x4 worldToLocalMatrix
        {
            get
            {
                if (recomputeMatrix)
                {
                    _localMatrix = Matrix4x4.PRS(_localPosition, _localRotation, _localScale);
                    _invLocalMatrix = _localMatrix.inverse;
                    recomputeMatrix = false;
                }
                if (parent == null)
                {
                    return _invLocalMatrix;
                }
                return _invLocalMatrix * parent._invLocalMatrix;
            }
        }

        public void SetParent(Transform parent)
        {
            if ((this.parent != parent) && (this.parent != null))
            {
                this.parent.children.Remove(this);
                this.parent = null;
            }
            this.parent = parent;
            if (parent != null)
            {
                parent.children.Add(this);
            }
        }

        public Transform GetParent()
        {
            return parent;
        }

        public Vector3 forward => rotation * Vector3.forward;
        public Vector3 back => rotation * Vector3.back;
        public Vector3 right => rotation * Vector3.right;
        public Vector3 left => rotation * Vector3.left;
        public Vector3 up => rotation * Vector3.up;
        public Vector3 down => rotation * Vector3.down;
    }
}
