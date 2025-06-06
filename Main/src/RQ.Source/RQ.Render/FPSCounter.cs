﻿using UnityEngine;

namespace RQ.Render
{
    public class FPSCounter : MonoBehaviour
    {
        public int frameRange = 60;
        public int AverageFPS { get; private set; }
        int[] fpsBuffer;
        int fpsBufferIndex;

        private void InitializeBuffer()
        {
            if (frameRange <= 0)
            {
                frameRange = 1;
            }
            fpsBuffer = new int[frameRange];
            fpsBufferIndex = 0;
        }

        public void Awake()
        {
            InitializeBuffer();
        }

        public void Update()
        {
            if (fpsBuffer == null || fpsBuffer.Length != frameRange)
            {
                InitializeBuffer();
            }
            UpdateBuffer();
            CalculateFPS();
        }

        private void UpdateBuffer()
        {
            fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
            if (fpsBufferIndex >= frameRange)
            {
                fpsBufferIndex = 0;
            }
        }

        void CalculateFPS()
        {
            int sum = 0;
            for (int i = 0; i < frameRange; i++)
            {
                sum += fpsBuffer[i];
            }
            AverageFPS = sum / frameRange;
        }
    }
}
