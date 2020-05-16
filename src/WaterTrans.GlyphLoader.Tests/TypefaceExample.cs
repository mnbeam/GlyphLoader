﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterTrans.GlyphLoader.Tests
{
    [TestClass]
    public class TypefaceExample
    {
        [TestMethod]
        public void CreateLigatureGlyphOutline()
        {
            string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Roboto-Regular.ttf");
            Typeface tf;
            using (var fontStream = System.IO.File.OpenRead(fontPath))
            {
                // Initialize stream only
                tf = new Typeface(fontStream);
            }

            var svg = new System.Text.StringBuilder();
            double unit = 100;
            double x = 20;
            double y = 20;
            string japaneseText = "fluent office";

            svg.AppendLine("<svg width='690' height='140' viewBox='0 0 690 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

            var glyphList = new List<ushort>();

            foreach (char c in japaneseText)
            {
                // Get glyph index
                ushort glyphIndex = tf.CharacterToGlyphMap[(int)c];

                // Get glyph outline
                var geometry = tf.GetGlyphOutline(glyphIndex, unit);

                // Get advanced width
                double advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;

                // Get advanced height
                double advanceHeight = tf.AdvanceHeights[glyphIndex] * unit;

                // Get baseline
                double baseline = tf.Baseline * unit;

                // Convert to path mini-language
                string miniLanguage = geometry.Figures.ToString(x, y + baseline);

                svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='0' />");
                x += advanceWidth;

                glyphList.Add(glyphIndex);
            }

            svg.AppendLine("</svg>");
            System.Diagnostics.Trace.WriteLine(svg.ToString());
            /* Result
            <svg width='690' height='140' viewBox='0 0 690 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>
            <path d='M40.31,95L31.28,95L31.28,49.15L22.93,49.15L22.93,42.17L31.28,42.17L31.28,36.75Q31.28,28.25 35.82,23.61Q40.36,18.97 48.66,18.97L48.66,18.97Q51.79,18.97 54.86,19.8L54.86,19.8L54.38,27.13Q52.08,26.69 49.49,26.69L49.49,26.69Q45.1,26.69 42.71,29.25Q40.31,31.82 40.31,36.6L40.31,36.6L40.31,42.17L51.59,42.17L51.59,49.15L40.31,49.15L40.31,95z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M71.37,20L71.37,95L62.33,95L62.33,20L71.37,20z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M118.63,95L118.44,89.78Q113.16,95.98 102.96,95.98L102.96,95.98Q94.51,95.98 90.09,91.07Q85.67,86.16 85.62,76.54L85.62,76.54L85.62,42.17L94.66,42.17L94.66,76.3Q94.66,88.31 104.42,88.31L104.42,88.31Q114.78,88.31 118.19,80.6L118.19,80.6L118.19,42.17L127.23,42.17L127.23,95L118.63,95z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M162.87,95.98L162.87,95.98Q152.13,95.98 145.39,88.92Q138.65,81.87 138.65,70.05L138.65,70.05L138.65,68.39Q138.65,60.53 141.66,54.35Q144.66,48.17 150.05,44.68Q155.45,41.19 161.75,41.19L161.75,41.19Q172.05,41.19 177.76,47.98Q183.48,54.77 183.48,67.41L183.48,67.41L183.48,71.17L147.69,71.17Q147.88,78.98 152.25,83.79Q156.62,88.6 163.36,88.6L163.36,88.6Q168.14,88.6 171.46,86.65Q174.79,84.7 177.28,81.47L177.28,81.47L182.79,85.77Q176.15,95.98 162.87,95.98z M161.75,48.61L161.75,48.61Q156.28,48.61 152.57,52.59Q148.86,56.57 147.98,63.75L147.98,63.75L174.44,63.75L174.44,63.07Q174.05,56.18 170.73,52.4Q167.41,48.61 161.75,48.61z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M193.93,42.17L202.47,42.17L202.76,48.81Q208.82,41.19 218.58,41.19L218.58,41.19Q235.33,41.19 235.48,60.09L235.48,60.09L235.48,95L226.45,95L226.45,60.04Q226.4,54.33 223.83,51.59Q221.27,48.86 215.85,48.86L215.85,48.86Q211.46,48.86 208.13,51.2Q204.81,53.54 202.96,57.35L202.96,57.35L202.96,95L193.93,95L193.93,42.17z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M252.32,29.38L261.36,29.38L261.36,42.17L271.22,42.17L271.22,49.15L261.36,49.15L261.36,81.91Q261.36,85.09 262.68,86.67Q263.99,88.26 267.17,88.26L267.17,88.26Q268.73,88.26 271.46,87.68L271.46,87.68L271.46,95Q267.9,95.98 264.53,95.98L264.53,95.98Q258.48,95.98 255.4,92.31Q252.32,88.65 252.32,81.91L252.32,81.91L252.32,49.15L242.71,49.15L242.71,42.17L252.32,42.17L252.32,29.38z ' fill='black' stroke='black' stroke-width='0' />
            <path d='' fill='black' stroke='black' stroke-width='0' />
            <path d='M304.13,68.73L304.13,68.1Q304.13,60.33 307.18,54.13Q310.23,47.93 315.68,44.56Q321.12,41.19 328.11,41.19L328.11,41.19Q338.9,41.19 345.56,48.66Q352.23,56.13 352.23,68.54L352.23,68.54L352.23,69.17Q352.23,76.88 349.27,83.01Q346.32,89.14 340.83,92.56Q335.33,95.98 328.2,95.98L328.2,95.98Q317.46,95.98 310.8,88.51Q304.13,81.04 304.13,68.73L304.13,68.73z M313.21,69.17L313.21,69.17Q313.21,77.96 317.29,83.28Q321.37,88.6 328.2,88.6L328.2,88.6Q335.09,88.6 339.14,83.21Q343.19,77.81 343.19,68.1L343.19,68.1Q343.19,59.4 339.07,54.01Q334.94,48.61 328.11,48.61L328.11,48.61Q321.42,48.61 317.31,53.94Q313.21,59.26 313.21,69.17z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M377.03,95L368,95L368,49.15L359.65,49.15L359.65,42.17L368,42.17L368,36.75Q368,28.25 372.54,23.61Q377.08,18.97 385.38,18.97L385.38,18.97Q388.51,18.97 391.58,19.8L391.58,19.8L391.09,27.13Q388.8,26.69 386.21,26.69L386.21,26.69Q381.82,26.69 379.42,29.25Q377.03,31.82 377.03,36.6L377.03,36.6L377.03,42.17L388.31,42.17L388.31,49.15L377.03,49.15L377.03,95z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M411.75,95L402.71,95L402.71,49.15L394.37,49.15L394.37,42.17L402.71,42.17L402.71,36.75Q402.71,28.25 407.26,23.61Q411.8,18.97 420.1,18.97L420.1,18.97Q423.22,18.97 426.3,19.8L426.3,19.8L425.81,27.13Q423.52,26.69 420.93,26.69L420.93,26.69Q416.53,26.69 414.14,29.25Q411.75,31.82 411.75,36.6L411.75,36.6L411.75,42.17L423.03,42.17L423.03,49.15L411.75,49.15L411.75,95z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M442.8,42.17L442.8,95L433.77,95L433.77,42.17L442.8,42.17z M433.04,28.15L433.04,28.15Q433.04,25.96 434.38,24.44Q435.72,22.93 438.36,22.93Q441,22.93 442.36,24.44Q443.73,25.96 443.73,28.15Q443.73,30.35 442.36,31.82Q441,33.28 438.36,33.28Q435.72,33.28 434.38,31.82Q433.04,30.35 433.04,28.15z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M478.45,88.6L478.45,88.6Q483.28,88.6 486.89,85.67Q490.51,82.74 490.9,78.35L490.9,78.35L499.44,78.35Q499.2,82.89 496.32,86.99Q493.44,91.09 488.63,93.54Q483.82,95.98 478.45,95.98L478.45,95.98Q467.66,95.98 461.28,88.77Q454.91,81.57 454.91,69.07L454.91,69.07L454.91,67.56Q454.91,59.84 457.74,53.84Q460.58,47.83 465.87,44.51Q471.17,41.19 478.4,41.19L478.4,41.19Q487.29,41.19 493.17,46.51Q499.05,51.84 499.44,60.33L499.44,60.33L490.9,60.33Q490.51,55.21 487.02,51.91Q483.53,48.61 478.4,48.61L478.4,48.61Q471.51,48.61 467.73,53.57Q463.95,58.53 463.95,67.9L463.95,67.9L463.95,69.61Q463.95,78.74 467.71,83.67Q471.46,88.6 478.45,88.6z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M531.52,95.98L531.52,95.98Q520.78,95.98 514.04,88.92Q507.3,81.87 507.3,70.05L507.3,70.05L507.3,68.39Q507.3,60.53 510.31,54.35Q513.31,48.17 518.71,44.68Q524.1,41.19 530.4,41.19L530.4,41.19Q540.7,41.19 546.42,47.98Q552.13,54.77 552.13,67.41L552.13,67.41L552.13,71.17L516.34,71.17Q516.53,78.98 520.9,83.79Q525.27,88.6 532.01,88.6L532.01,88.6Q536.8,88.6 540.12,86.65Q543.44,84.7 545.93,81.47L545.93,81.47L551.45,85.77Q544.8,95.98 531.52,95.98z M530.4,48.61L530.4,48.61Q524.93,48.61 521.22,52.59Q517.51,56.57 516.63,63.75L516.63,63.75L543.1,63.75L543.1,63.07Q542.71,56.18 539.38,52.4Q536.06,48.61 530.4,48.61z ' fill='black' stroke='black' stroke-width='0' />
            </svg>
            */

            x = 20;
            y = 20;
            svg.Clear();

            // Get ligature glyph map
            var ligaMap = tf.GetLigatureSubstitutionMap("latn", "DFLT", "liga");

            // Create ligature first glyph and glyph count dictionary
            var firstGlyphDic = new Dictionary<ushort, int>();
            foreach (var key in ligaMap.Keys)
            {
                if (!firstGlyphDic.ContainsKey(key[0]) || firstGlyphDic[key[0]] < key.Length)
                {
                    firstGlyphDic[key[0]] = key.Length;
                }
            }

            var ligatureGlyphList = new List<ushort>();

            for (int i = 0; i < glyphList.Count; i++)
            {
                bool found = false;

                // Glyph has ligature
                if (firstGlyphDic.ContainsKey(glyphList[i]))
                {
                    // Get maximum length of ligature
                    var ligatureMaxLength = firstGlyphDic[glyphList[i]];

                    // Determines whether ligature is possible from the maximum length.
                    for (int j = ligatureMaxLength; j > 0; j--)
                    {
                        if (glyphList.Count >= i + j)
                        { 
                            ushort[] key = glyphList.GetRange(i, j).ToArray();
                            if (ligaMap.ContainsKey(key))
                            {
                                ligatureGlyphList.Add(ligaMap[key]);
                                i += j - 1;
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (!found)
                {
                    ligatureGlyphList.Add(glyphList[i]);
                }
            }

            svg.AppendLine("<svg width='690' height='140' viewBox='0 0 690 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

            foreach (var glyphIndex in ligatureGlyphList)
            {
                // Get glyph outline
                var geometry = tf.GetGlyphOutline(glyphIndex, unit);

                // Get advanced width
                double advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;

                // Get advanced height
                double advanceHeight = tf.AdvanceHeights[glyphIndex] * unit;

                // Get baseline
                double baseline = tf.Baseline * unit;

                // Convert to path mini-language
                string miniLanguage = geometry.Figures.ToString(x, y + baseline);

                svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='0' />");
                x += advanceWidth;
            }

            svg.AppendLine("</svg>");
            System.Diagnostics.Trace.WriteLine(svg.ToString());
            /* Result
            <svg width='690' height='140' viewBox='0 0 690 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>
            <path d='M59.84,95L59.84,27.67Q53.79,26.69 50.08,26.69L50.08,26.69Q40.31,26.69 40.31,36.94L40.31,36.94L40.31,42.17L51.59,42.17L51.59,49.15L40.31,49.15L40.31,95L31.28,95L31.28,49.15L22.93,49.15L22.93,42.17L31.28,42.17L31.28,36.41Q31.33,27.96 36.02,23.47Q40.7,18.97 49.35,18.97L49.35,18.97Q54.28,18.97 68.88,21.95L68.88,21.95L68.88,95L59.84,95z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M116.44,95L116.24,89.78Q110.97,95.98 100.76,95.98L100.76,95.98Q92.31,95.98 87.9,91.07Q83.48,86.16 83.43,76.54L83.43,76.54L83.43,42.17L92.46,42.17L92.46,76.3Q92.46,88.31 102.23,88.31L102.23,88.31Q112.58,88.31 116,80.6L116,80.6L116,42.17L125.03,42.17L125.03,95L116.44,95z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M160.67,95.98L160.67,95.98Q149.93,95.98 143.19,88.92Q136.46,81.87 136.46,70.05L136.46,70.05L136.46,68.39Q136.46,60.53 139.46,54.35Q142.46,48.17 147.86,44.68Q153.25,41.19 159.55,41.19L159.55,41.19Q169.85,41.19 175.57,47.98Q181.28,54.77 181.28,67.41L181.28,67.41L181.28,71.17L145.49,71.17Q145.68,78.98 150.05,83.79Q154.42,88.6 161.16,88.6L161.16,88.6Q165.95,88.6 169.27,86.65Q172.59,84.7 175.08,81.47L175.08,81.47L180.6,85.77Q173.96,95.98 160.67,95.98z M159.55,48.61L159.55,48.61Q154.08,48.61 150.37,52.59Q146.66,56.57 145.78,63.75L145.78,63.75L172.25,63.75L172.25,63.07Q171.86,56.18 168.54,52.4Q165.21,48.61 159.55,48.61z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M191.73,42.17L200.27,42.17L200.57,48.81Q206.62,41.19 216.39,41.19L216.39,41.19Q233.13,41.19 233.28,60.09L233.28,60.09L233.28,95L224.25,95L224.25,60.04Q224.2,54.33 221.64,51.59Q219.07,48.86 213.65,48.86L213.65,48.86Q209.26,48.86 205.94,51.2Q202.62,53.54 200.76,57.35L200.76,57.35L200.76,95L191.73,95L191.73,42.17z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M250.13,29.38L259.16,29.38L259.16,42.17L269.02,42.17L269.02,49.15L259.16,49.15L259.16,81.91Q259.16,85.09 260.48,86.67Q261.8,88.26 264.97,88.26L264.97,88.26Q266.53,88.26 269.27,87.68L269.27,87.68L269.27,95Q265.7,95.98 262.33,95.98L262.33,95.98Q256.28,95.98 253.2,92.31Q250.13,88.65 250.13,81.91L250.13,81.91L250.13,49.15L240.51,49.15L240.51,42.17L250.13,42.17L250.13,29.38z ' fill='black' stroke='black' stroke-width='0' />
            <path d='' fill='black' stroke='black' stroke-width='0' />
            <path d='M301.93,68.73L301.93,68.1Q301.93,60.33 304.99,54.13Q308.04,47.93 313.48,44.56Q318.93,41.19 325.91,41.19L325.91,41.19Q336.7,41.19 343.36,48.66Q350.03,56.13 350.03,68.54L350.03,68.54L350.03,69.17Q350.03,76.88 347.08,83.01Q344.12,89.14 338.63,92.56Q333.13,95.98 326.01,95.98L326.01,95.98Q315.26,95.98 308.6,88.51Q301.93,81.04 301.93,68.73L301.93,68.73z M311.02,69.17L311.02,69.17Q311.02,77.96 315.09,83.28Q319.17,88.6 326.01,88.6L326.01,88.6Q332.89,88.6 336.94,83.21Q341,77.81 341,68.1L341,68.1Q341,59.4 336.87,54.01Q332.74,48.61 325.91,48.61L325.91,48.61Q319.22,48.61 315.12,53.94Q311.02,59.26 311.02,69.17z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M374.83,95L365.8,95L365.8,49.15L357.45,49.15L357.45,42.17L365.8,42.17L365.8,36.75Q365.8,28.25 370.34,23.61Q374.88,18.97 383.18,18.97L383.18,18.97Q386.31,18.97 389.38,19.8L389.38,19.8L388.9,27.13Q386.6,26.69 384.01,26.69L384.01,26.69Q379.62,26.69 377.23,29.25Q374.83,31.82 374.83,36.6L374.83,36.6L374.83,42.17L394.37,42.17L394.37,37.68Q394.37,28.84 399.42,23.91Q404.47,18.97 413.7,18.97L413.7,18.97Q419.17,18.97 427.52,21.95L427.52,21.95L426.01,29.57Q419.9,27.13 414.34,27.13L414.34,27.13Q408.53,27.13 405.96,29.72Q403.4,32.3 403.4,37.58L403.4,37.58L403.4,42.17L414.24,42.17L414.24,49.15L403.4,49.15L403.4,95L394.37,95L394.37,49.15L374.83,49.15L374.83,95z M431.96,42.17L431.96,95L422.93,95L422.93,42.17L431.96,42.17z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M467.9,88.6L467.9,88.6Q472.73,88.6 476.35,85.67Q479.96,82.74 480.35,78.35L480.35,78.35L488.9,78.35Q488.65,82.89 485.77,86.99Q482.89,91.09 478.08,93.54Q473.27,95.98 467.9,95.98L467.9,95.98Q457.11,95.98 450.74,88.77Q444.37,81.57 444.37,69.07L444.37,69.07L444.37,67.56Q444.37,59.84 447.2,53.84Q450.03,47.83 455.33,44.51Q460.62,41.19 467.85,41.19L467.85,41.19Q476.74,41.19 482.62,46.51Q488.51,51.84 488.9,60.33L488.9,60.33L480.35,60.33Q479.96,55.21 476.47,51.91Q472.98,48.61 467.85,48.61L467.85,48.61Q460.97,48.61 457.18,53.57Q453.4,58.53 453.4,67.9L453.4,67.9L453.4,69.61Q453.4,78.74 457.16,83.67Q460.92,88.6 467.9,88.6z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M520.98,95.98L520.98,95.98Q510.23,95.98 503.5,88.92Q496.76,81.87 496.76,70.05L496.76,70.05L496.76,68.39Q496.76,60.53 499.76,54.35Q502.76,48.17 508.16,44.68Q513.55,41.19 519.85,41.19L519.85,41.19Q530.16,41.19 535.87,47.98Q541.58,54.77 541.58,67.41L541.58,67.41L541.58,71.17L505.79,71.17Q505.99,78.98 510.36,83.79Q514.73,88.6 521.46,88.6L521.46,88.6Q526.25,88.6 529.57,86.65Q532.89,84.7 535.38,81.47L535.38,81.47L540.9,85.77Q534.26,95.98 520.98,95.98z M519.85,48.61L519.85,48.61Q514.38,48.61 510.67,52.59Q506.96,56.57 506.08,63.75L506.08,63.75L532.55,63.75L532.55,63.07Q532.16,56.18 528.84,52.4Q525.52,48.61 519.85,48.61z ' fill='black' stroke='black' stroke-width='0' />
            </svg>
            */
        }

        [TestMethod]
        public void CreateHorizontalGlyphOutlineIshindenshin()
        {
            string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "NotoSerifJP-Regular.otf");
            Typeface tf;
            using (var fontStream = System.IO.File.OpenRead(fontPath))
            {
                // Initialize stream only
                tf = new Typeface(fontStream);
            }

            var svg = new System.Text.StringBuilder();
            double unit = 100;
            double x = 20;
            double y = 20;
            string japaneseText = "以心伝心";

            svg.AppendLine("<svg width='440' height='140' viewBox='0 0 440 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

            foreach (char c in japaneseText)
            {
                // Get glyph index
                ushort glyphIndex = tf.CharacterToGlyphMap[(int)c];

                // Get glyph outline
                var geometry = tf.GetGlyphOutline(glyphIndex, unit);

                // Get advanced width
                double advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;

                // Get advanced height
                double advanceHeight = tf.AdvanceHeights[glyphIndex] * unit;

                // Get baseline
                double baseline = tf.Baseline * unit;

                // Convert to path mini-language
                string miniLanguage = geometry.Figures.ToString(x, y + baseline);

                svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='0' />");
                x += advanceWidth;
            }

            svg.AppendLine("</svg>");
            System.Diagnostics.Trace.WriteLine(svg.ToString());
            /* Result
            <svg width='440' height='140' viewBox='0 0 440 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>
            <path d='M57.1,41.2C63.2,46.9 71.4,56.2 73.9,63.3C81.8,68.4 86.2,52 58.1,40.4z M71.1,82.5C61.8,85.9 52.5,89.3 44.5,92.1L43.5,32.9C46.1,32.5 46.9,31.4 47,30L37,29.2L38,94.3C31.8,96.4 26.6,98.1 23.3,98.9L27.6,107.4C28.6,107 29.5,105.9 29.7,104.7C48,96.4 61.6,89.2 71.6,84z M98.5,29.4C97.6,67 93.2,93.3 46.9,113.8L48,115.7C68.8,108.2 81.9,99.3 90.3,88.9C97.8,96.3 106.5,106.4 109.6,114.2C117.9,119.5 121.7,102.5 91.9,86.8C102.7,72 104.6,54.3 105.6,33.6C107.8,33.3 109,32.2 109.3,30.7z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M136.5,57.2C136.7,70.6 131.1,82.5 125.2,87.1C123.4,89 122.5,91.5 123.9,93.3C125.6,95.3 129.6,94 132.5,90.9C136.9,86.2 142.4,74.8 138.3,57.2z M152.8,26.5L152.2,28.1C165.1,33.7 174,41.9 177.3,47.2C185.2,51.4 189.5,33.5 152.8,26.5z M195.6,56L194.6,57.1C205.5,66.4 209.1,81.1 209.4,90.2C216.7,98.7 225.3,72.3 195.6,56z M151,46.9L151,104.4C151,110.8 153.6,112.5 162.9,112.5L176.7,112.5C196.2,112.5 200,111.4 200,107.9C200,106.5 199.3,105.7 196.9,104.9L196.7,86.7L195.4,86.7C193.9,94.9 192.5,102.1 191.6,104.1C191.1,105.2 190.6,105.7 189.2,105.9C187.3,106.1 182.8,106.1 176.8,106.1L163.7,106.1C158.4,106.1 157.6,105.3 157.6,103L157.6,50.7C159.8,50.3 160.8,49.3 161,48z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M307.2,38.6C308.7,38.6 309.6,38.1 309.9,37.1C306.6,33.9 301.1,29.6 301.1,29.6L296.4,35.7L257.2,35.7L257.9,38.6z M246.5,24.2C241.4,43.2 232.7,62.2 224,74.3L225.4,75.3C229.8,71 234,65.8 237.9,59.9L237.9,115.7L239.1,115.7C241.5,115.7 244.3,114.1 244.4,113.5L244.4,54C246.2,53.7 247.1,53 247.4,52.1L243.4,50.6C247.1,44 250.3,36.8 253,29.3C255.2,29.4 256.4,28.6 256.9,27.5z M303.6,63.1L249,63.1L249.7,66.1L273.3,66.1C270.5,77.2 265.5,93.4 261.9,103C256.5,103.5 252,103.8 248.8,103.9L252.9,113C253.9,112.8 254.9,112.1 255.5,111C276.6,107 292,103.8 303.8,101.1C305.9,105.3 307.5,109.4 308.3,113C316.2,119.6 321.1,100.5 292,79.9L290.6,80.7C294.5,85.7 299.1,92.4 302.7,99.1C288.8,100.6 275.5,101.9 265,102.8C270.3,93.3 277.2,78 281.9,66.1L314.8,66.1C316,66.1 317,65.6 317.3,64.5C314,61.4 308.5,57.1 308.5,57.1z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M336.5,57.2C336.7,70.6 331.1,82.5 325.2,87.1C323.4,89 322.5,91.5 323.9,93.3C325.6,95.3 329.6,94 332.5,90.9C336.9,86.2 342.4,74.8 338.3,57.2z M352.8,26.5L352.2,28.1C365.1,33.7 374,41.9 377.3,47.2C385.2,51.4 389.5,33.5 352.8,26.5z M395.6,56L394.6,57.1C405.5,66.4 409.1,81.1 409.4,90.2C416.7,98.7 425.3,72.3 395.6,56z M351,46.9L351,104.4C351,110.8 353.6,112.5 362.9,112.5L376.7,112.5C396.2,112.5 400,111.4 400,107.9C400,106.5 399.3,105.7 396.9,104.9L396.7,86.7L395.4,86.7C393.9,94.9 392.5,102.1 391.6,104.1C391.1,105.2 390.6,105.7 389.2,105.9C387.3,106.1 382.8,106.1 376.8,106.1L363.7,106.1C358.4,106.1 357.6,105.3 357.6,103L357.6,50.7C359.8,50.3 360.8,49.3 361,48z ' fill='black' stroke='black' stroke-width='0' />
            </svg>
            */
        }

        [TestMethod]
        public void CreateVerticalGlyphOutlineFurinkazan()
        {
            string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "NotoSansJP-Regular.otf");
            Typeface tf;
            using (var fontStream = System.IO.File.OpenRead(fontPath))
            {
                // Initialize stream only
                tf = new Typeface(fontStream);
            }

            // Get vertical glyph map
            var vertMap = tf.GetSingleSubstitutionMap("DFLT", "DFLT", "vert");

            var svg = new System.Text.StringBuilder();
            double unit = 100;
            double x = 20;
            double y = 20;
            string japaneseText = "【風林火山】";

            svg.AppendLine("<svg width='140' height='640' viewBox='0 0 140 640' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

            foreach (char c in japaneseText)
            {
                // Get glyph index
                ushort glyphIndex = tf.CharacterToGlyphMap[(int)c];

                // Get vertical glyph index
                glyphIndex = vertMap.ContainsKey(glyphIndex) ? vertMap[glyphIndex] : glyphIndex;

                // Get glyph outline
                var geometry = tf.GetGlyphOutline(glyphIndex, unit);

                // Get advanced width
                double advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;

                // Get advanced height
                double advanceHeight = tf.AdvanceHeights[glyphIndex] * unit;

                // Get baseline
                double baseline = tf.Baseline * unit;

                // Convert to path mini-language
                string miniLanguage = geometry.Figures.ToString(x, y + baseline);

                svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='0' />");
                y += advanceHeight;
            }

            svg.AppendLine("</svg>");
            System.Diagnostics.Trace.WriteLine(svg.ToString());
            /* Result
            <svg width='140' height='640' viewBox='0 0 140 640' xmlns='http://www.w3.org/2000/svg' version='1.1'>
            <path d='M116.6,86.6L23.4,86.6L23.4,116.6L23.9,116.6C33.1,105.7 49.7,96.8 70,96.8C90.3,96.8 106.9,105.7 116.1,116.6L116.6,116.6z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M54.5,180.3L54.5,165.1L65.8,165.1L65.8,180.3z M83.8,165.1L83.8,180.3L72.4,180.3L72.4,165.1z M79.9,190.6C81.8,193.2 83.6,196.1 85.3,199.1L72.4,199.9L72.4,186.2L90.1,186.2L90.1,159.1L72.4,159.1L72.4,149.4C79.8,148.5 86.7,147.3 92.2,145.8L87,140.4C77.5,143 60.2,145 45.7,146C46.5,147.5 47.4,150 47.7,151.5C53.5,151.3 59.7,150.8 65.8,150.2L65.8,159.1L48.5,159.1L48.5,186.2L65.8,186.2L65.8,200.3C56.8,200.8 48.7,201.3 42.5,201.6L43,208.4C54.8,207.5 71.8,206.3 88.4,205.1C89.8,207.9 90.8,210.6 91.4,212.8L97.5,210.6C95.8,204.5 90.8,195.3 85.6,188.7z M35.7,129.8L35.7,161.8C35.7,177.1 34.6,197.4 23.9,211.6C25.6,212.5 28.6,214.5 29.8,215.8C41.1,200.7 42.8,178 42.8,161.8L42.8,136.6L96.8,136.6C97.1,180.2 97,215.6 109.2,215.6C114.3,215.6 115.8,210.6 116.5,197.7C115.1,196.5 113.2,194.4 111.9,192.4C111.7,201.1 111.2,207.9 109.9,207.9C104,207.9 103.8,166.9 103.9,129.8z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M67.2,273.3C64.8,270.4 54.2,258.2 50.9,255.1L50.9,252.8L65.1,252.8L65.1,245.7L50.9,245.7L50.9,224.3L43.6,224.3L43.6,245.7L25.8,245.7L25.8,252.8L42.3,252.8C38.5,266.6 30.8,282 23.3,290.4C24.6,292.2 26.4,295.1 27.3,297.3C33.3,290.3 39.3,278.7 43.6,266.7L43.6,315.5L50.9,315.5L50.9,264C55,269.2 60.1,276.1 62.3,279.7z M113.6,252.8L113.6,245.7L94.7,245.7L94.7,224.3L87.4,224.3L87.4,245.7L69.4,245.7L69.4,252.8L85.8,252.8C81.2,268.8 71.9,285.1 62.4,294.4C63.8,296.1 65.8,298.9 66.8,300.9C74.6,293.1 82.1,280.1 87.4,266.4L87.4,315.5L94.7,315.5L94.7,266.1C99.1,279.2 104.9,291.4 111,299.1C112.3,297.1 114.9,294.6 116.7,293.3C108.7,284.6 101.1,268.6 96.6,252.8z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M73.5,325.8L65.6,325.8L65.6,357.9C65.6,369.5 58.7,396.8 25.2,409.6C26.9,411.1 29.3,414.2 30.3,415.7C58.5,404.1 67.6,382 69.5,372.3C71.5,381.9 81.2,404.8 110.1,415.7C111.2,413.7 113.4,410.5 115,408.9C80.6,396.7 73.5,369.3 73.5,357.9z M102.7,344.3C99.4,353 93.1,365 88.2,372.3L94.4,375.2C99.5,368.1 105.9,356.8 110.7,347.5z M40.4,344.5C38.8,355.6 35.4,366.4 27.4,372.4L33.8,376.8C42.7,370 46,357.9 47.8,346z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M101.9,447.9L101.9,498.8L73.4,498.8L73.4,426.4L65.8,426.4L65.8,498.8L38.3,498.8L38.3,448L30.8,448L30.8,514.4L38.3,514.4L38.3,506.3L101.9,506.3L101.9,514L109.5,514L109.5,447.9z ' fill='black' stroke='black' stroke-width='0' />
            <path d='M116.6,553.4L116.6,523.4L116.1,523.4C106.9,534.3 90.3,543.2 70,543.2C49.7,543.2 33.1,534.3 23.9,523.4L23.4,523.4L23.4,553.4z ' fill='black' stroke='black' stroke-width='0' />
            </svg>
            */
        }
    }
}
